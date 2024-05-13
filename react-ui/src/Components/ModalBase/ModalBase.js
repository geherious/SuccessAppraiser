import './ModalBase.css';
import clsx from "clsx";

const ModalBase = ({isActive, setIsActive, children}) => {
  return (
    <div className={clsx('modal', isActive && 'active')} onClick={() => setIsActive(false)}>
        <div className={clsx("modal-content", isActive && 'active')} onClick={(e) => e.stopPropagation()}>
            {children}
        </div>
    </div>
  )
}

export default ModalBase