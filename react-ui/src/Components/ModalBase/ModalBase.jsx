import './ModalBase.css';
import clsx from "clsx";

const ModalBase = ({ isActive, setIsActive, onModalClose, children }) => {

  const onClose = () => {
    onModalClose();
    setIsActive(false);
  }


  return (
    <div className={clsx('modal', isActive && 'active')} onClick={onClose}>
      <div className={clsx("modal-content", isActive && 'active')} onClick={(e) => e.stopPropagation()}>
        {children}
      </div>
    </div>
  )
}

export default ModalBase